// src/app/language.service.ts
import { Injectable, effect, signal } from '@angular/core';
import { TranslocoService } from '@jsverse/transloco';

@Injectable({
   providedIn: 'root' 
})

export class LanguageService {
  current = signal<'en' | 'fr'>('en');

  constructor(private t: TranslocoService) {
    this.current.set((this.t.getActiveLang() as 'en' | 'fr') || 'en');

    effect(() => {
      const lang = this.current();
      this.t.setActiveLang(lang);
      try {
        localStorage.setItem('lang', lang);
      }
      catch { }
      document.documentElement.lang = lang;
    });
  }

  set(lang: 'en' | 'fr') {
    this.current.set(lang);
  }
  get() {
    return this.current();
  }
}

