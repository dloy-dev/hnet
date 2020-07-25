"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var testing_1 = require("@angular/core/testing");
var stories_component_1 = require("./stories.component");
var stories_service_1 = require("./stories.service");
var forms_1 = require("@angular/forms");
var table_1 = require("@angular/material/table");
var paginator_1 = require("@angular/material/paginator");
var animations_1 = require("@angular/platform-browser/animations");
var rxjs_1 = require("rxjs");
var FakeStoriesService = /** @class */ (function () {
    function FakeStoriesService() {
    }
    FakeStoriesService.prototype.getStories = function (page, size, query) {
        return rxjs_1.of(this.buildStoryData(page, size, query));
    };
    FakeStoriesService.prototype.buildStoryData = function (page, size, query) {
        var story1 = { author: "a1", id: 1, title: "t1", url: "u1", time: new Date() };
        var story2 = { author: "a2", id: 2, title: "t2", url: "u2", time: new Date() };
        var story3 = { author: "a3", id: 3, title: "t3", url: "u3", time: new Date() };
        var story4 = { author: "a4", id: 4, title: "t4", url: "u4", time: new Date() };
        var story5 = { author: "a5", id: 5, title: "t5", url: "u5", time: new Date() };
        var story6 = { author: "a6", id: 6, title: "t6", url: "u6", time: new Date() };
        var story7 = { author: "a7", id: 7, title: "t7", url: "u7", time: new Date() };
        var story8 = { author: "a8", id: 8, title: "t8", url: "u8", time: new Date() };
        var story9 = { author: "a9", id: 9, title: "t9", url: "u9", time: new Date() };
        var story10 = { author: "a10", id: 10, title: "t10", url: "u10", time: new Date() };
        var story11 = { author: "a11", id: 11, title: "t11", url: "u11", time: new Date() };
        var story12 = { author: "a12", id: 12, title: "t12", url: "u12", time: new Date() };
        var story13 = { author: "a13", id: 13, title: "t13", url: "u13", time: new Date() };
        var story14 = { author: "a14", id: 14, title: "t14", url: "u14", time: new Date() };
        var story15 = { author: "a15", id: 15, title: "t15", url: "u15", time: new Date() };
        var story16 = { author: "a16", id: 16, title: "t16", url: "u16", time: new Date() };
        var story17 = { author: "a17", id: 17, title: "t17", url: "u17", time: new Date() };
        var story18 = { author: "a18", id: 18, title: "t18", url: "u18", time: new Date() };
        var story19 = { author: "a19", id: 19, title: "t19", url: "u19", time: new Date() };
        var story20 = { author: "a20", id: 20, title: "t20", url: "u20", time: new Date() };
        var storyList = [];
        if (page == 1 && !query) {
            storyList = [story1, story2, story3, story4, story5, story6, story7, story8, story9, story10];
        }
        else if (page == 2 && !query) {
            storyList = [story11, story12, story13, story14, story15, story16, story17, story18, story19, story20];
        }
        var storyParams = { totalElements: 10, pageNumber: page, pageSize: size };
        return { stories: storyList, parameters: storyParams };
    };
    return FakeStoriesService;
}());
describe('StoriesComponent', function () {
    var component;
    var fixture;
    beforeEach(testing_1.async(function () {
        testing_1.TestBed.configureTestingModule({
            imports: [
                forms_1.FormsModule,
                table_1.MatTableModule,
                paginator_1.MatPaginatorModule,
                animations_1.BrowserAnimationsModule,
            ],
            declarations: [stories_component_1.StoriesComponent],
            providers: [
                {
                    provide: stories_service_1.StoryService,
                    useClass: FakeStoriesService
                }
            ]
        }).compileComponents();
        fixture = testing_1.TestBed.createComponent(stories_component_1.StoriesComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    }));
    it('should create', function () {
        expect(component).toBeTruthy();
    });
    it('should call getStories with intial params when ngOnInit is called', function () {
        var getStoriesSpy = spyOn(component, 'getStories');
        component.ngOnInit();
        expect(getStoriesSpy).toHaveBeenCalled();
        expect(getStoriesSpy).toHaveBeenCalledWith("1", "10", "");
    });
    it('should call getStories with next page params when nextPage is called', function () {
        var event = new paginator_1.PageEvent();
        event.pageIndex = 1;
        event.pageSize = 10;
        var getStoriesSpy = spyOn(component, 'getStories');
        component.nextPage(event);
        expect(getStoriesSpy).toHaveBeenCalled();
        expect(getStoriesSpy).toHaveBeenCalledWith("2", "10", "");
    });
    it('should call getStories with search params when submitFilter is called', function () {
        var getStoriesSpy = spyOn(component, 'getStories');
        component.itemsPerPage = 10;
        component.searchText = "searching";
        component.submitFilter();
        expect(getStoriesSpy).toHaveBeenCalled();
        expect(getStoriesSpy).toHaveBeenCalledWith("1", "10", "searching");
    });
    it('should reset paginator to first page when submitFilter is called', function () {
        var firstPageSpy = spyOn(component.paginator, 'firstPage');
        component.itemsPerPage = 10;
        component.searchText = "searching";
        component.submitFilter();
        expect(firstPageSpy).toHaveBeenCalled();
    });
});
//# sourceMappingURL=stories.component.spec.js.map