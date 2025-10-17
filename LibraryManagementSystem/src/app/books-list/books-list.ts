import { Component, Input, OnChanges, OnInit, SimpleChanges, ChangeDetectorRef, ViewChild, TemplateRef } from '@angular/core';
import { BooksModel } from '../../Models/BooksModel';
import { BookListService } from '../books-list/book-list-service';
import { PublishersModel } from '../../Models/PublihsersModel';
import { NgbPaginationModule, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { filter } from 'rxjs';
import { AddEdit } from '../add-edit/add-edit';
import { AddEditService } from '../add-edit/add-edit-service';
import { Router } from '@angular/router';
import { ToastService } from '../toast-container/toast-service';
import { ExportService,BookExportRow } from '../services/export-service';

@Component({
  selector: 'app-books-list',
  standalone: false,
  templateUrl: './books-list.html',
  styleUrls: ['./books-list.css']
})
export class BooksList implements OnChanges, OnInit {
  Books: BooksModel[] = [];
  TotalRecords: number = 0;
  loading: boolean = false;
  pendingBookName = '';

  @ViewChild('confirmTpl') confirmTpl!: TemplateRef<any>;
  @Input() filters: BooksModel = new BooksModel();

  constructor(private booksService: BookListService,
    private _changeDetectorRef: ChangeDetectorRef,
    private modalService: NgbModal,
    private addEditService: AddEditService,
    private router: Router,
    private toast: ToastService,
    private exporter : ExportService
  ) { }

  ngOnInit(): void {
    debugger;
    this.getBooks();
  }



  ngOnChanges(changes: SimpleChanges): void {
    if (changes['filters']) {
      debugger;
      this.getBooks();
    }
  }


  sortBy(column: string) {
    debugger;
    this.filters.PageNo = 1;
    if (this.filters.SortColumn === column) {
      this.filters.SortOrder = this.filters.SortOrder === 'asc' ? 'desc' : 'asc';
    } else {
      this.filters.SortColumn = column;
      this.filters.SortOrder = 'asc';
    }

    this.getBooks();
  }

  private toExportRows(src: any[]): BookExportRow[] {
    return src.map(b => ({
      BookId: b.BookId,
      BookName: b.BookName,
      PublisherName: b.PublisherName,
      DepartmentName: b.DepartmentName,
      SupplierName: b.SupplierName
    }));
  }
  exportCsvPage() {
    this.exporter.exportBooksCsv(this.toExportRows(this.Books), 'books-page.csv');
  }
  exportXlsxPage() {
    this.exporter.exportBooksXlsx(this.toExportRows(this.Books), 'books-page.xlsx');
  }
  exportPdfPage() {
    this.exporter.exportBooksPdf(this.toExportRows(this.Books), 'books-page.pdf',false);
  }
  
  openAddEditModal(bookId: number = 0) {
    this.addEditService.AddEdit(bookId).subscribe({
      next: (response) => {
        const modalRef = this.modalService.open(AddEdit, {
          
          size: 'lg',
          backdrop: 'static',
          keyboard: false
        });


        modalRef.componentInstance.model = response;
        modalRef.componentInstance.publishers = response.PublishersList;
        modalRef.componentInstance.departments = response.DepartmentsList;
        modalRef.componentInstance.suppliers = response.SuppliersList;
        modalRef.componentInstance.users = response.UsersList;

        modalRef.result.then(
          () => this.getBooks()
        );
      },
      error: () => {
        this.toast.error('Error loading book data');
      }
    });
  }

  openAddEditPage(bookId: number = 0) {
    debugger;
    if (bookId > 0) {
      this.router.navigate(['/books/edit', bookId]);
    } else {
      this.router.navigate(['/books/add']);
    }
  }

  async deleteBook(book: BooksModel) {
    this.pendingBookName = book.BookName || '';

    let ok = false;
    try {
      await this.modalService.open(this.confirmTpl, { backdrop: 'static', centered: true, keyboard: false }).result;
      ok = true;
    } catch {
      ok = false;
    }
    if (!ok) return;
    this.booksService.BookDelete(book.BookId).subscribe({
      next: () => {
        this.toast.error(`"${book.BookName}" has been deleted successfully.`, { delay: 4000 });
        this.getBooks();
      },
      error: () => {
        debugger;
        this.toast.error('Error deleting the book. Please try again.');
      }
    });
  }



  getBooks() {
    this.loading = true;
    sessionStorage.setItem('bookFilters', JSON.stringify(this.filters));
    this.booksService.Bookgetlist(this.filters).subscribe(
      (response: any) => {
        debugger;
        this.Books = response.BookList || [];
        this.TotalRecords = response.TotalBooksCount;
        this.loading = false;
        this._changeDetectorRef.markForCheck();
      },
      (error) => {
        this.toast.error('Error fetching books');
        this.loading = false;
      }
    );
  }
}
