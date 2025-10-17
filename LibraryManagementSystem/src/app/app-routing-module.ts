import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BooksList } from './books-list/books-list';
import { AddEdit } from './add-edit/add-edit';
import { SearchFilters } from './search-filters/search-filters';
import { Login } from './login/login';
import { BookIssues } from './book-issues/book-issues';
import { authGuard } from './guards/auth-guard';
import { roleGuard } from './guards/role-guard';
import { pendingChangesGuard } from './guards/pending-changes-guard';

// 
const routes: Routes = [
  { path: '', component: Login},
  { path: 'books', component: SearchFilters,canActivate: [authGuard]},
  { path: 'books/add', component: AddEdit ,canActivate: [authGuard,roleGuard] , data: { roles: ['Admin','Librarian']}, canDeactivate: [pendingChangesGuard] },
  { path: 'books/edit/:id', component: AddEdit ,canActivate: [authGuard,roleGuard], data: { roles: ['Admin','Librarian']},canDeactivate: [pendingChangesGuard]},
  { path: 'books/login', component: Login },
  { path: 'books/bookIssues', component: BookIssues ,canActivate: [authGuard, roleGuard], data: { roles: ['Admin']}}, 
];

@NgModule({
  imports:[RouterModule.forRoot(routes,{useHash:true,onSameUrlNavigation:'reload'})], 
  exports: [RouterModule]
})
export class AppRoutingModule { }
