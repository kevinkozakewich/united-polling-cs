import { Component, OnInit, Inject } from '@angular/core';
import { MsalService, MsalBroadcastService, MSAL_GUARD_CONFIG, MsalGuardConfiguration } from '@azure/msal-angular';
import { AuthenticationResult, EventMessage, EventType, InteractionStatus, InteractionType, PopupRequest, RedirectRequest } from '@azure/msal-browser';
import { Router } from '@angular/router';
import { DOCUMENT } from '@angular/common';
import { Subject } from 'rxjs';
import { filter, takeUntil } from 'rxjs/operators';

@Component({
    selector: 'app-header',
    templateUrl: './app-header.html',
})

export class AppHeaderComponent implements OnInit {
    private readonly _destroying$ = new Subject<void>();
	logging: boolean = false;
	interval: any;
    loggedIn = false;
    loginName: string = "";
	userLoggedInData: any = [];
	displayHeaderContents: boolean = false;

	constructor(
        @Inject(DOCUMENT) private document: Document,
		@Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
		private authService: MsalService,
		private msalBroadcastService: MsalBroadcastService,
        private router: Router) {}
  
	ngOnInit(): void {
        this.setloggedIn();

        this.authService.instance.enableAccountStorageEvents(); // Optional - This will enable ACCOUNT_ADDED and ACCOUNT_REMOVED events emitted when a user logs in or out of another tab or window

        this.msalBroadcastService.msalSubject$
            .pipe(
                filter((msg: EventMessage) => msg.eventType === EventType.ACCOUNT_ADDED || msg.eventType === EventType.ACCOUNT_REMOVED),
            )
            .subscribe((result: EventMessage) => {
                if (this.authService.instance.getAllAccounts().length === 0) {
                    window.location.pathname = "/";
                } else {
                    this.setloggedIn();
                }
            });

        this.msalBroadcastService.inProgress$
            .pipe(
                filter((status: InteractionStatus) => status === InteractionStatus.None),
                takeUntil(this._destroying$)
            )
            .subscribe(() => {
                this.setloggedIn();
                this.checkAndSetActiveAccount();
                this.getClaims(this.authService.instance.getActiveAccount()?.idTokenClaims);
                this.redirectIfNeeded();
            })
    }

    setloggedIn() {
        this.loggedIn = this.authService.instance.getAllAccounts().length > 0;
    }
    getClaims(claims: any) {
        if (claims) {
            this.loginName = claims["name"];
        }
    }
    redirectIfNeeded()
    {
        if(this.document.location.pathname === "/" || this.document.location.pathname === "/loading") {
            // If waiting at the login screen, then either login or go to home if successful.
            if(this.loggedIn) { this.router.navigate(['home']); }
            else { this.performLogin(); }
        }
        else 
        {
            // If anywhere else in the application but not logged in, bring back to the main screen and log in.
            if(!this.loggedIn) { this.router.navigate(['/']); }
        }
    }

    checkAndSetActiveAccount() {
        /**
         * If no active account set but there are accounts signed in, sets first account to active account
         * To use active account set here, subscribe to inProgress$ first in your component
         * Note: Basic usage demonstrated. Your app may require more complicated account selection logic
         */
        let activeAccount = this.authService.instance.getActiveAccount();

        if (!activeAccount && this.authService.instance.getAllAccounts().length > 0) {
            let accounts = this.authService.instance.getAllAccounts();
            // add your code for handling multiple accounts here
            this.authService.instance.setActiveAccount(accounts[0]);
        }
    }

    login(event: Event) {
        event.preventDefault();
        this.performLogin();
    }

    performLogin() {
        if (this.msalGuardConfig.interactionType === InteractionType.Popup) {
            if (this.msalGuardConfig.authRequest) {
                this.authService.loginPopup({ ...this.msalGuardConfig.authRequest } as PopupRequest)
                    .subscribe((response: AuthenticationResult) => {
                        this.authService.instance.setActiveAccount(response.account);
                    });
            } else {
                this.authService.loginPopup()
                    .subscribe((response: AuthenticationResult) => {
                        this.authService.instance.setActiveAccount(response.account);
                    });
            }
        } else {
            if (this.msalGuardConfig.authRequest) {
                this.authService.loginRedirect({ ...this.msalGuardConfig.authRequest } as RedirectRequest);
            } else {
                this.authService.loginRedirect();
            }
        }
    }

    logout(event: Event) {
        event.preventDefault();
        const activeAccount = this.authService.instance.getActiveAccount() || this.authService.instance.getAllAccounts()[0];

        if (this.msalGuardConfig.interactionType === InteractionType.Popup) {
            this.authService.logoutPopup({
                account: activeAccount,
            });
        } else {
            this.authService.logoutRedirect({
                account: activeAccount,
            });
        }
    }

    // Unsubscribe to events when component is destroyed.
    ngOnDestroy(): void {
        this._destroying$.next(undefined);
        this._destroying$.complete();
    }
}
