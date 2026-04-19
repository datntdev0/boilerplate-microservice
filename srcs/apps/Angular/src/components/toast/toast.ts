import { Component, OnInit, OnDestroy, signal } from '@angular/core';
import { ToastService, ToastConfig } from './toast-service';
import { Subscription } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-toast',
  imports: [CommonModule],
  templateUrl: './toast.html',
})
export class ToastComponent implements OnInit, OnDestroy {
  private subscription?: Subscription;
  
  public toastsSignal = signal<ToastConfig[]>([]);
  
  constructor(private toastService: ToastService) {}

  ngOnInit(): void {
    this.subscription = this.toastService.toasts$
      .subscribe((toasts: ToastConfig[]) => this.toastsSignal.set(toasts));
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  remove(id: string): void {
    this.toastService.remove(id);
  }

  getToastClass(type: string): string {
    const baseClass = 'toast';
    const typeClass = `toast-${type}`;
    return `${baseClass} ${typeClass}`;
  }
}
