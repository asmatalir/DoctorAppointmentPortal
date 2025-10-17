import { Injectable } from '@angular/core';
import { saveAs } from 'file-saver';
import * as XLSX from 'xlsx';

// pdfmake UMD builds
import pdfMake from 'pdfmake/build/pdfmake';
import * as pdfFonts from 'pdfmake/build/vfs_fonts';

// Version-agnostic font hookup (handles both { vfs } and { pdfMake: { vfs } })
const fontsAny: any = pdfFonts as any;
(pdfMake as any).vfs = fontsAny.vfs ?? fontsAny.pdfMake?.vfs;

export interface BookExportRow {
  BookId: number | string;
  BookName: string;
  PublisherName?: string;
  DepartmentName?: string;
  SupplierName?: string;
}

@Injectable({ providedIn: 'root' })
export class ExportService {

  // ---------- CSV ----------
  exportBooksCsv(rows: BookExportRow[], filename = 'books.csv'): void {
    const headers = ['ID', 'Book Name', 'Publisher', 'Department', 'Supplier'];
    const lines = [
      headers.join(','), ...rows.map(r => [ r.BookId, r.BookName, r.PublisherName ?? '', r.DepartmentName ?? '', r.SupplierName ?? '' ]  .map(v => this.csvEscape(String(v))).join(',') )
    ];
    const blob = new Blob([lines.join('\r\n')], { type: 'text/csv;charset=utf-8;' });
    saveAs(blob, filename);
  }

  private csvEscape(s: string): string {
    const needsQuotes = /[",\r\n]/.test(s);
    const escaped = s.replace(/"/g, '""');
    return needsQuotes ? `"${escaped}"` : escaped;
  }

  // ---------- Excel (XLSX) ----------
  exportBooksXlsx(rows: BookExportRow[], filename = 'books.xlsx'): void {
    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(rows, {
      header: ['BookId', 'BookName', 'PublisherName', 'DepartmentName', 'SupplierName']
    });

    // Column widths (characters)
    (ws as any)['!cols'] = [
      { wch: 8 }, 
      { wch: 30 }, 
      { wch: 24 }, 
      { wch: 24 }, 
      { wch: 24 } 
    ];

    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Books');

    const out: ArrayBuffer = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
    saveAs(new Blob([out]), filename);
  }

  // ---------- PDF (pdfmake) ----------
  exportBooksPdf(rows: BookExportRow[], filename = 'books.pdf', landscape = false): void {
    const body: any[] = [
      [
        { text: 'ID', bold: true },
        { text: 'Book Name', bold: true },
        { text: 'Publisher', bold: true },
        { text: 'Department', bold: true },
        { text: 'Supplier', bold: true }
      ],
      ...rows.map(r => [
        String(r.BookId),
        r.BookName ?? '',
        r.PublisherName ?? '',
        r.DepartmentName ?? '',
        r.SupplierName ?? ''
      ])
    ];

    const doc: any = {
      pageSize: 'A4',
      pageOrientation: landscape ? 'landscape' : 'portrait',
      pageMargins: [18, 24, 18, 24],
      content: [
        { text: 'Books List', style: 'h1' },
        {
          table: {
            headerRows: 1,
            widths: ['auto', '*', '*', '*', '*'],
            body
          },
          layout: 'lightHorizontalLines'
        }
      ],
      styles: {
        h1: { fontSize: 16, bold: true, margin: [0, 0, 0, 10] }
      },
      footer: (currentPage: number, pageCount: number) => ({
        columns: [
          { text: `Generated: ${new Date().toLocaleString()}`, alignment: 'left', margin: [18, 0, 0, 0] },
          { text: `${currentPage} / ${pageCount}`, alignment: 'right', margin: [0, 0, 18, 0] }
        ]
      })
    };

    pdfMake.createPdf(doc).download(filename);
  }
}
