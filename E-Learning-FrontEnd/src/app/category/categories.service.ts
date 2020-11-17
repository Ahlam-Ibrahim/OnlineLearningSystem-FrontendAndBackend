import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { Category } from './category.model';
import { Course } from './course.model';

@Injectable({providedIn: 'root'})

export class CategoriesService {
    constructor(private http: HttpClient) { }

    getCategories(){
        return this.http.get<Category>("http://localhost:54277/api/categories")
        .pipe(map(responseData => {
          //convert json to array of Category objects
          const categoryArray : Category[] = [];
          for(const key in responseData){
            if(responseData.hasOwnProperty(key)){
              categoryArray.push(responseData[key]);
          }
        }
        return categoryArray;
      })
      );
    }

    getCourses(){
        return this.http.get<Course>("http://localhost:54277/api/courses")
        .pipe(map(responseData => {
          const courseyArray : Course[] = [];
          for(const key in responseData){
            if(responseData.hasOwnProperty(key)){
                courseyArray.push(responseData[key]);
          }
        }
        return courseyArray;
      })
      );
    }

    
    GetAllCategoriesOfACourse(index: number){
        return this.http.get<Category>("http://localhost:54277/api/categories/courses/"+index)
        .pipe(map(responseData => {
          const categoryArray : Category[] = [];
          for(const key in responseData){
            if(responseData.hasOwnProperty(key)){
                categoryArray.push(responseData[key]);
          }
        }
        return categoryArray;
      })
      );
    }
}