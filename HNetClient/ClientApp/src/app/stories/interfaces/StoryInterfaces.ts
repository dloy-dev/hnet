export interface Story {
  author: string;
  id: number;
  title: string;
  url: string;
  time: Date;
}

export interface StoryParameters {
  pageNumber: number;
  pageSize: number;
  totalElements: number;
}

export interface StoryResponse {
  stories: Story[];
  parameters: StoryParameters;
}
