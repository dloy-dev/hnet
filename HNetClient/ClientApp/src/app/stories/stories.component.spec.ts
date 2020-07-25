import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { StoriesComponent } from './stories.component';
import { StoryService } from './stories.service';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { of } from 'rxjs';
import { } from 'jasmine';

class FakeStoriesService {

  getStories(page, size, query) {
    return of(this.buildStoryData(page, size, query));
  }

  buildStoryData(page, size, query) {
    let story1 = { author: "a1", id: 1, title: "t1", url: "u1", time: new Date() };
    let story2 = { author: "a2", id: 2, title: "t2", url: "u2", time: new Date() };
    let story3 = { author: "a3", id: 3, title: "t3", url: "u3", time: new Date() };
    let story4 = { author: "a4", id: 4, title: "t4", url: "u4", time: new Date() };
    let story5 = { author: "a5", id: 5, title: "t5", url: "u5", time: new Date() };
    let story6 = { author: "a6", id: 6, title: "t6", url: "u6", time: new Date() };
    let story7 = { author: "a7", id: 7, title: "t7", url: "u7", time: new Date() };
    let story8 = { author: "a8", id: 8, title: "t8", url: "u8", time: new Date() };
    let story9 = { author: "a9", id: 9, title: "t9", url: "u9", time: new Date() };
    let story10 = { author: "a10", id: 10, title: "t10", url: "u10", time: new Date() };
    let story11 = { author: "a11", id: 11, title: "t11", url: "u11", time: new Date() };
    let story12 = { author: "a12", id: 12, title: "t12", url: "u12", time: new Date() };
    let story13 = { author: "a13", id: 13, title: "t13", url: "u13", time: new Date() };
    let story14 = { author: "a14", id: 14, title: "t14", url: "u14", time: new Date() };
    let story15 = { author: "a15", id: 15, title: "t15", url: "u15", time: new Date() };
    let story16 = { author: "a16", id: 16, title: "t16", url: "u16", time: new Date() };
    let story17 = { author: "a17", id: 17, title: "t17", url: "u17", time: new Date() };
    let story18 = { author: "a18", id: 18, title: "t18", url: "u18", time: new Date() };
    let story19 = { author: "a19", id: 19, title: "t19", url: "u19", time: new Date() };
    let story20 = { author: "a20", id: 20, title: "t20", url: "u20", time: new Date() };

    let storyList = [];

    if (page == 1 && !query) {
      storyList = [story1, story2, story3, story4, story5, story6, story7, story8, story9, story10];
    }
    else if (page == 2 && !query) {
      storyList = [story11, story12, story13, story14, story15, story16, story17, story18, story19, story20];
    }

    let storyParams = { totalElements: 10, pageNumber: page, pageSize: size };

    return { stories: storyList, parameters: storyParams }
  }
}

describe('StoriesComponent', () => {
  let component: StoriesComponent;
  let fixture: ComponentFixture<StoriesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        FormsModule,
        MatTableModule,
        MatPaginatorModule,
        BrowserAnimationsModule,
      ],
      declarations: [StoriesComponent],
      providers: [
        {
          provide: StoryService,
          useClass: FakeStoriesService
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(StoriesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call getStories with intial params when ngOnInit is called', () => {
    const getStoriesSpy = spyOn(component, 'getStories');

    component.ngOnInit();

    expect(getStoriesSpy).toHaveBeenCalled();
    expect(getStoriesSpy).toHaveBeenCalledWith("1", "10", "");
  });

  it('should call getStories with next page params when nextPage is called', () => {
    let event = new PageEvent();
    event.pageIndex = 1;
    event.pageSize = 10;

    const getStoriesSpy = spyOn(component, 'getStories');

    component.nextPage(event);

    expect(getStoriesSpy).toHaveBeenCalled();
    expect(getStoriesSpy).toHaveBeenCalledWith("2", "10", "");
  });

  it('should call getStories with search params when submitFilter is called', () => {
    const getStoriesSpy = spyOn(component, 'getStories');
    component.itemsPerPage = 10;
    component.searchText = "searching";

    component.submitFilter();

    expect(getStoriesSpy).toHaveBeenCalled();
    expect(getStoriesSpy).toHaveBeenCalledWith("1", "10", "searching");
  });

  it('should reset paginator to first page when submitFilter is called', () => {
    const firstPageSpy = spyOn(component.paginator, 'firstPage');
    component.itemsPerPage = 10;
    component.searchText = "searching";

    component.submitFilter();

    expect(firstPageSpy).toHaveBeenCalled();
  });
});
