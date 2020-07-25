import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { StoryResponse } from './interfaces/StoryInterfaces';

@Injectable({
  providedIn: 'root',
})

export class StoryService {
  public http: HttpClient;
  public baseUrl: string;
  public storyData: StoryResponse;

  constructor(
    http: HttpClient,
    @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
  }

  getStories(page, size, query) {
    return this.http.get<StoryResponse>(`${this.baseUrl}api/stories?pageNumber=${page}&pageSize=${size}&search=${query}`);
  }
}
