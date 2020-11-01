import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UserComponent } from './user/user.component';
import { StudentComponent } from './user/student/student.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { LoginComponent } from './user/login/login.component';
import { HomeComponent } from './home/home.component';
import { CategoryComponent } from './category/category.component';
import { CourseComponent } from './category/course/course.component';
import { SectionComponent } from './category/course/section/section.component';
import { VideoComponent } from './category/course/section/video/video.component';
import { StatusComponent } from './category/course/status/status.component';
import { AboutComponent } from './about/about.component';
import { ContactComponent } from './contact/contact.component';
import { YouTubePlayerModule } from "@angular/youtube-player";
import { UserService } from './shared/user.service';

@NgModule({
  declarations: [
    AppComponent,
    UserComponent,
    StudentComponent,
    RegistrationComponent,
    LoginComponent,
    HomeComponent,
    CategoryComponent,
    CourseComponent,
    SectionComponent,
    VideoComponent,
    StatusComponent,
    AboutComponent,
    ContactComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    YouTubePlayerModule,
    HttpClientModule,
    ReactiveFormsModule,   
    BrowserAnimationsModule,
    ToastrModule.forRoot()
  ],
  providers: [UserService],
  bootstrap: [AppComponent]
})
export class AppModule { }
