import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class InfoSharingService {
  private dataSub = new BehaviorSubject<any>(null);
  data$ = this.dataSub.asObservable();

  sendData(data: any): void {
    this.dataSub.next(data);
  }
}
