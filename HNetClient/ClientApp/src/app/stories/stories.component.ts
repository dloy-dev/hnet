import { Component, OnInit, ViewChild } from '@angular/core';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { StoryService } from './stories.service';
import { Story, StoryResponse } from './interfaces/StoryInterfaces';

@Component({
  selector: 'app-stories',
  templateUrl: './stories.component.html',
  styleUrls: ['./stories.component.css']
})
export class StoriesComponent implements OnInit {
  @ViewChild('paginator', { static: false }) paginator: MatPaginator;
  public stories: Story[];
  public totalElements: number = 0;
  public searchText: string = "";
  public storyService: StoryService;
  public storyData: StoryResponse;
  public itemsPerPage: number = 10;

  constructor(private service: StoryService) {
    this.storyService = service;
  }

  ngOnInit(): void {
    this.getStories("1", "10", "")
  }

  nextPage(event: PageEvent) {
    this.itemsPerPage = event.pageSize;
    this.getStories((event.pageIndex + 1).toString(), event.pageSize.toString(), this.searchText);
  }

  submitFilter() {
    this.getStories("1", this.itemsPerPage.toString(), this.searchText);
    this.paginator.firstPage();
  }

  getStories(page, size, query) {
    this.storyService.getStories(page, size, query).subscribe((data: StoryResponse) => {
      this.stories = data.stories;
      this.totalElements = data.parameters.totalElements;
    });
  }
}
