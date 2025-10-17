import { FileDetailModel } from "./FileDetailModel";
import { BookDetailModel } from "./BookDetailModel";

export class BookIssueModel {
    IssueId: number;
    MemberId: number;
    MemberName : string;
    IssueDate: string;     
    DueDate: string;       
    LoanPeriod: number;
    IsActive: boolean;
    CreatedBy: number;
    CreatedOn: string;     
    ModifiedBy: number;
    ModifiedOn: string;   
    BookId: number;

    DeletedExistingFiles: string[] = [];
    // SelectedBooks: BookDetailModel[] = [];
  }