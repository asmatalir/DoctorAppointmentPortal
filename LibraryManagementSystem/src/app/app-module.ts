import { NgModule, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection   } from '@angular/core';
import { BrowserModule, provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { provideHttpClient, withFetch } from '@angular/common/http';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { NgbModule,NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { RouterModule } from '@angular/router';
import { AuthInterceptor } from './interceptor/authentication';
import { LoaderInterceptor } from './interceptor/loader-interceptor';
import { TranslocoModule, provideTransloco, provideTranslocoConfig } from '@jsverse/transloco';
import { TranslocoHttpLoader } from './services/transloco-http.loader';
import { NgbToastModule } from '@ng-bootstrap/ng-bootstrap';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { BooksList } from './books-list/books-list';
import { SearchFilters } from './search-filters/search-filters';
import { AddEdit } from './add-edit/add-edit';
import { Login } from './login/login';
import { Navbar } from './navbar/navbar';
import { Loader } from './loader/loader';
import { BookIssues } from './book-issues/book-issues';
import { ToastContainer } from './toast-container/toast-container';


@NgModule({
  declarations: [
    App,
    BooksList,
    SearchFilters,
    AddEdit,
    Login,
    Navbar,
    Loader,
    BookIssues,
    ToastContainer
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    RouterModule,
    FormsModule,
    HttpClientModule,
    NgbModule,
    NgMultiSelectDropDownModule.forRoot(),
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    TranslocoModule,
    NgbToastModule,
    NgbDropdownModule
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideHttpClient(withFetch()),
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoaderInterceptor, multi: true },
    provideTransloco({
      config: {
        availableLangs: ['en', 'fr'],
        defaultLang: initialLang(),
        reRenderOnLangChange: true
      },
      loader: TranslocoHttpLoader
    })

  ],
  bootstrap: [App]
})

export class AppModule { }


function initialLang(): 'en' | 'fr' {
  try {
    return (localStorage.getItem('lang') as 'en' | 'fr') || 'en'; 
  } 
  catch {
     return 'en'; 
  }
}

