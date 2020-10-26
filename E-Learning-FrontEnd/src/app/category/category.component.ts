import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Category } from './category.model';
import { map } from 'rxjs/operators';
import { CategoriesService } from './categories.service';
import { Course } from './course.model';


@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styles: [
  ]
})
export class CategoryComponent implements OnInit {
  loadedCategories: Category[] = [];
  loadedCourses: Course[] = [];
  CategoriesOfACourse: Category[] = [];

  constructor(private http: HttpClient, private categoriesService: CategoriesService) { }

  ngOnInit(): void {
    this.GetCategories();
    this.GetCourses();
  }

  private GetCategories(){
    this.categoriesService.getCategories()
    .subscribe(categories => {
      this.loadedCategories = categories;
    });
  }

  private GetCourses(){
    this.categoriesService.getCourses()
    .subscribe(courses => {
      this.loadedCourses = courses;
      console.log(this.loadedCourses);
    });
  }

  private GetAllCategoriesOfACourse(index: number){
    this.categoriesService.GetAllCategoriesOfACourse(index)
    .subscribe(categories => {
      this.CategoriesOfACourse = categories;
      console.log(this.CategoriesOfACourse);
    });
  }
}
