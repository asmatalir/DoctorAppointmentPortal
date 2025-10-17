import { Component,Input, ChangeDetectorRef, SimpleChanges, OnChanges, OnInit, ViewChild} from '@angular/core';
import { BooksModel } from '../../Models/BooksModel';
import { PublishersModel } from '../../Models/PublihsersModel';
import { DepartmentsModel } from '../../Models/DepartmentsModel';
import { SuppliersModel } from '../../Models/SuppliersModel';
import { AddEditService } from './add-edit-service';
import { UsersModel } from '../../Models/UsersModel';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { TranslocoService } from '@jsverse/transloco';
import { ToastService } from '../toast-container/toast-service';

@Component({
  selector: 'app-add-edit',
  standalone: false,
  templateUrl: './add-edit.html',
  styleUrl: './add-edit.css'
})
export class AddEdit {
 model : BooksModel = new BooksModel();
 publishers : PublishersModel[] = [];
 departments : DepartmentsModel[] = [];
 suppliers : SuppliersModel[] = [];
 users : UsersModel[] = [];
 @Input() bookId!: number;

 @ViewChild('addEditForm') form?: NgForm;
  private formSaved = false;

 constructor(private addEditService : AddEditService,
  // public activeModal: NgbActiveModal,
  private _changeDetectorRef: ChangeDetectorRef,
  private route: ActivatedRoute,
  private router: Router,
  private translocoService: TranslocoService,
  private toast : ToastService
 ) {}

 ngOnInit(): void {
  debugger;
  this.bookId = Number(this.route.snapshot.paramMap.get('id')) || 0;
  this.loadBookDetails(this.bookId);
}


loadBookDetails(id : number = 0){
  debugger;
  this.addEditService.AddEdit(id).subscribe({
    next: (response) => {
      debugger;
      if (id > 0) {
        this.model = response;
      } else {
        this.model = new BooksModel();
      }

      this.publishers = response.PublishersList;
      this.departments = response.DepartmentsList;
      this.suppliers = response.SuppliersList;
      this.users = response.UsersList;

      this._changeDetectorRef.detectChanges();
    },
    error : (err) => {
      this.toast.error('Error Loading Book Details');
    }
  })
}

canDeactivate(): boolean {
  debugger;
  if (this.form?.dirty) {
    return confirm('Are you sure you want to discard your changes?');
  }
  return true;
}

onClick(form: NgForm) {
  if (form.invalid) {
    const msg = this.translocoService.translate('bookForm.formValidation.message');
    this.toast.warning(msg);
    return;
  }

  this.addEditService.SaveAddEdit(this.model).subscribe({
    next: (response: any) => {
      if (response.success) {
        this.toast.success('Book Details Saved successfully');
        this.router.navigate(['/books']);
        this.formSaved=true;
        form.form.markAsPristine();
        // this.activeModal.close(this.model);
      } else {
        this.toast.error('Error Saving Book Details');
      }
    },
    error: (err) => {
      this.toast.error('Error Saving Book Details');
    }
  });
}


}
