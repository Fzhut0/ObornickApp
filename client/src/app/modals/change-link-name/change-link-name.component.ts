import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Link } from 'src/app/_models/link';
import { LinksService } from 'src/app/_services/links.service';

@Component({
  selector: 'app-change-link-name',
  templateUrl: './change-link-name.component.html',
  styleUrls: ['./change-link-name.component.css']
})
export class ChangeLinkNameComponent implements OnInit {

  link: Link | null = null;
  currentName: string | undefined = '';
  newName: string = '';

  constructor(private linksService: LinksService, public bsModalRef: BsModalRef) {}

  ngOnInit(): void {
    this.currentName = this.link?.customName;
  }

  updateLinkName(newName: string)
  {
    if (this.currentName)
    {
      this.linksService.updateLinkName(this.currentName, newName).subscribe({
        next: () => {
          this.bsModalRef.hide();
        }
      });
      }
  }

}
