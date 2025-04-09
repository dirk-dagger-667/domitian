import { Injectable, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs/internal/Subscription';

@Injectable({ providedIn: 'root' })
export abstract class UserAdminBase implements OnDestroy {
  readonly subs: Subscription[] = [];

  ngOnDestroy(): void {
    this.subs.forEach((sub) => {
      if (sub !== undefined) sub.unsubscribe();
    });
  }
}
