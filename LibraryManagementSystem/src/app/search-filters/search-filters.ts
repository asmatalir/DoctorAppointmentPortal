import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { BooksModel } from '../../Models/BooksModel';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { SearchFiltersService } from '../search-filters/search-filters-service';
import { PublishersModel } from '../../Models/PublihsersModel'


@Component({
  selector: 'app-search-filters',
  standalone: false,
  templateUrl: './search-filters.html',
  styleUrl: './search-filters.css'
})
export class SearchFilters implements OnInit {

  filtersToSend: BooksModel = new BooksModel();
  Publishers: PublishersModel[] = [];

  constructor(private searchFilterService: SearchFiltersService) { }

  dropdownSettings: IDropdownSettings = {
    singleSelection: false,
    idField: 'PublisherId',
    textField: 'PublisherName',
    selectAllText: 'Select All',
    unSelectAllText: 'Unselect All',
    itemsShowLimit: 2,
    allowSearchFilter: true
  };

  ngOnInit(): void {
    this.GetPublihsers();

    const savedFilters = sessionStorage.getItem('bookFilters');
    if (savedFilters) {
      this.filtersToSend = JSON.parse(savedFilters);
    }



  }



  GetPublihsers() {
    this.searchFilterService.GetLists().subscribe(
      (data: any) => {
        debugger;
        this.Publishers = data.PublishersList || [];
      },
      (error) => {
        console.error('Error fetching books', error);
      }
    );
  }

  onSearch() {
    debugger;
    this.filtersToSend.PageNo = 1;
    this.filtersToSend.SelectedPublisher = this.filtersToSend.SelectedPublishers?.length ? this.filtersToSend.SelectedPublishers.map(p => p.PublisherId).join(',') : '';
    this.filtersToSend = { ...this.filtersToSend };
    sessionStorage.setItem('bookFilters', JSON.stringify(this.filtersToSend));

  }



  ClearFilters() {
      sessionStorage.removeItem('bookFilters');

    this.filtersToSend = new BooksModel();
  }

}
