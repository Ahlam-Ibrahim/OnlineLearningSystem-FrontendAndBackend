import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AboutComponent } from './about/about.component';
import { CategoryComponent } from './category/category.component';
import { CourseComponent } from './category/course/course.component';
import { ContactComponent } from './contact/contact.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './user/login/login.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { StudentComponent } from './user/student/student.component';

const routes: Routes = [
  {
    path:'student', component:StudentComponent
  },
  {
    path:'category', component:CategoryComponent
  },
  {
    path:'about', component:AboutComponent
  },
  {
    path:'registration', component:RegistrationComponent
  },
  {
    path:'login', component:LoginComponent
  },
  {
    path:'course', component:CourseComponent
  },
  {
    path:'contact', component:ContactComponent
  },
  {
    path:'profile', component:StudentComponent
  },

  {
    path:'**', component:HomeComponent
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
