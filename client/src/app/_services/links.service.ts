import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Category } from '../_models/category';
import { Link } from '../_models/link';

@Injectable({
  providedIn: 'root'
})
export class LinksService {

  baseUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) { }

  addLink(link: Link)
  {
    const requestBody = {
      customName: link.customName,
      savedUrl: link.savedUrl,
      categoryId: link.categoryId
    }
    return this.httpClient.post(this.baseUrl + 'CheckLaterLink/addlink', requestBody, {responseType: 'text'});
  }

  deleteLink(name: string)
  {
    const params = new HttpParams({
      fromObject: {
        name: name,
      }
    })
    return this.httpClient.delete(this.baseUrl + 'CheckLaterLink/deletelink', {params: params})
  }

  markLinkAsWatched(link: Link)
  {
    return this.httpClient.put(this.baseUrl + 'CheckLaterLink/setlinkviewed', link);
  }

  updateLinkCategory(link: Link, newCategoryId: number)
  {
     const requestBody = {
      customName: link.customName,
      savedUrl: link.savedUrl,
      categoryId: newCategoryId
    }
    return this.httpClient.put(this.baseUrl + 'CheckLaterLink/updatelinkcategory', requestBody, {responseType: 'text'});
  }

  updateLinkName(currentName: string, newName: string) {
  const params = new HttpParams()
    .set('currentName', currentName)
    .set('newName', newName);

  return this.httpClient.put(this.baseUrl + 'CheckLaterLink/updatelinkname', null, { params: params, responseType: 'text' });
}
 
}
