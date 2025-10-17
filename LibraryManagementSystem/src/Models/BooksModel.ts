import { PublishersModel } from "./PublihsersModel"; 

export class BooksModel {
    BookNameSearch?: string = '';
    SelectedPublisher: string='';
    PageNo: number = 1;
    PageSize: number = 5;
    SortColumn?: string='';
    SortOrder?: string='';
    SelectedIsActive?: number=-1;
    SelectedPublishers?: PublishersModel[];





    BookId : number=0;
    BookName? : string='';
    PublisherId : number | null =null;
    PublisherName? : string='';
    DepartmentId : number | null=null;
    DepartmentName? : string='';
    SupplierId : number | null=null;
    SupplierName? : string='';
    NoOfCopies : number | null =null ;
    IsActive : Boolean= false;
    CreatedBy : number | null  = null ;
  }