import { TestBed } from '@angular/core/testing';
import { StoryService } from "./stories.service";
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('StoryService', () => {
  let service: StoryService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        { provide: 'BASE_URL', useValue: 'http://localhost' },
        StoryService
      ]
    });

    service = TestBed.get(StoryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
