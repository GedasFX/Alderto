import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NewsComponent } from './news.component';
import { DocumentationComponent } from './documentation.component';
import { HomeLayoutComponent } from "./home.layout";

const routes: Routes = [
  {
    path: '',
    component: HomeLayoutComponent,
    data: {
      title: 'Home'
    },
    children: [
      {
        path: '',
        redirectTo: 'news'
      },
      {
        path: 'news',
        component: NewsComponent,
        data: {
          title: 'News'
        }
      },
      {
        path: 'documentation',
        component: DocumentationComponent,
        data: {
          title: 'Documentation'
        }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeRoutingModule { }
