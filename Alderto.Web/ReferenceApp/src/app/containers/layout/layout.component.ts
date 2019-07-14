import { Component, OnDestroy, Inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { navItems } from '../../_nav';

@Component({
    selector: 'app-dashboard',
    templateUrl: './layout.component.html'
})

export class LayoutComponent implements OnDestroy {
    public navItems = navItems;
    public sidebarMinimized = true;
    public element: HTMLElement;
    private readonly changes: MutationObserver;
    constructor(@Inject(DOCUMENT) document?: any) {

        this.changes = new MutationObserver((mutations) => {
            this.sidebarMinimized = document.body.classList.contains('sidebar-minimized');
        });
        this.element = document.body;
        this.changes.observe(this.element as Element, {
            attributes: true,
            attributeFilter: ['class']
        });
    }

    ngOnDestroy(): void {
        this.changes.disconnect();
    }
}
