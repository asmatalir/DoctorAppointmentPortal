import { Component, ChangeDetectorRef } from '@angular/core';
import { BookIssuesService } from './book-issues-service';
import { TranslocoService } from '@jsverse/transloco';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BookIssueModel } from '../../Models/BookIssuesModel';
import { BookDetailModel } from '../../Models/BookDetailModel';
import { FileDetailModel } from '../../Models/FileDetailModel';
import { MembersModel } from '../../Models/MembersModel';
import { BooksModel } from '../../Models/BooksModel';
import { ToastService } from '../toast-container/toast-service';

@Component({
  selector: 'app-book-issues',
  standalone: false,
  templateUrl: './book-issues.html',
  styleUrl: './book-issues.css'
})
export class BookIssues {

  constructor(
    private bookIssuesService: BookIssuesService,
    private _changeDetectorRef: ChangeDetectorRef,
    private route: ActivatedRoute,
    private router: Router,
    private toast: ToastService
  ) { }

  model: BookIssueModel = new BookIssueModel();
  SelectedBooks: BookDetailModel[] = [];
  UploadedFiles: FileDetailModel[] = [];
  NewUploadedFiles: FileDetailModel[] = [];
  MembersList: MembersModel[] = [];
  BooksList: BooksModel[] = [];
  DeletedExistingFiles: string[] = [];

  MAX_SIZE = 5 * 1024 * 1024;
  bookId: number = 1068;
  ngOnInit(): void {
    debugger;
    this.loadBookIsseuDetails(this.bookId);
  }

  loadBookIsseuDetails(id: number = 0) {
    debugger;
    this.bookIssuesService.AddEditIssueBook(id).subscribe({
      next: (response) => {
        if (id > 0) {
          this.model = response;
          this.model.IssueDate = response.IssueDate ? response.IssueDate.split('T')[0] : '';
          this.model.DueDate = response.DueDate ? response.DueDate.split('T')[0] : '';
        } else {
          this.model = new BookIssueModel();
          const today = new Date();
          this.model.IssueDate = today.toISOString().split('T')[0];
        }
        console.log(this.model);
        debugger;
        this.BooksList = response.BookList || [];
        this.SelectedBooks = response.SelectedBooks || [];
        this.UploadedFiles = response.UploadedFiles || [];
        this.MembersList = response.MembersList || [];
        this.DeletedExistingFiles = response.DeletedExistingFiles || [];


        this._changeDetectorRef.detectChanges();
      },
      error: (err) => {
        this.toast.error('Error Loading Book Issue Details');
      }
    });
  }


  onSubmit() {
    const formData = new FormData();
    console.log("Model length in chars:", JSON.stringify(this.model).length);
    this.model.DeletedExistingFiles = this.DeletedExistingFiles;
    formData.append("model", JSON.stringify(this.model));

    this.NewUploadedFiles.forEach((fileDetail: FileDetailModel) => {
      formData.append('files', fileDetail.file, fileDetail.file.name);

    });
    
    const deletedFiles = formData.get("DeletedExistingFiles");
    console.log("DeletedExistingFiles from FormData:", deletedFiles);

    this.bookIssuesService.SaveAddEditIssueBook(formData).subscribe({
      next: (response: any) => {
        debugger;
        if (response.success) {
          this.toast.success('Book Issued Successfully');
          this.router.navigate(['/books']);
        } else {
          debugger;
          this.toast.error('Error saving book issue');
        }
      },
      error: (err) => {
        this.toast.error('Error saving book issue');
      }
    });
  }

  removeExistingFile(index: number) {
    debugger;
    const file = this.UploadedFiles[index];
    this.DeletedExistingFiles.push(file.FilePath);
    this.UploadedFiles.splice(index, 1);
  }

  removeNewFile(index: number) {
    this.NewUploadedFiles.splice(index, 1);
  }

  removeBookRow(index: number) {
    this.SelectedBooks.splice(index, 1);
  }

  addBookRow() {
    this.SelectedBooks.push({ BookId: 0, Quantity: 1 } as BookDetailModel);
  }

  onFileSelected(event: any) {
    debugger;
    const files: FileList = event.target.files;

    for (let i = 0; i < files.length; i++) {
      const file = files[i];

      if (file.size > this.MAX_SIZE) {
        this.toast.error(`File "${file.name}" is too large. Max allowed size is 5 MB.`, {delay : 4000});
        continue;
      }
      const isInUploaded = this.UploadedFiles.some(f => f.FileName === file.name);
      const isInNewFiles = this.NewUploadedFiles.some(f => f.FileName === file.name);

      if (isInUploaded || isInNewFiles) {
        this.toast.error(`File "${file.name}" is already selected.`, {delay : 4000});
        continue;
      }

      this.NewUploadedFiles.push({ file: file, FileName: file.name, FilePath: '' });
    }
    event.target.value = '';
  }

  DownloadFile(file: any) {
    this.bookIssuesService.DownloadFile(file.FilePath).subscribe(res => {
      const url = URL.createObjectURL(res);
      const a = document.createElement('a');
      a.href = url;
      a.download = file.FileName; 
      a.click();
      URL.revokeObjectURL(url);
    });
  }


}
