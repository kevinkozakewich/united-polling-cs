/**
 * This file contains authentication parameters. Contents of this file
 * is roughly the same across other MSAL.js libraries. These parameters
 * are used to initialize Angular and MSAL Angular configurations in
 * in app.module.ts file.
 */

import { LogLevel, Configuration, BrowserCacheLocation } from '@azure/msal-browser';

const isIE = window.navigator.userAgent.indexOf("MSIE ") > -1 || window.navigator.userAgent.indexOf("Trident/") > -1;

export const msalConfig: Configuration = {
    auth: {
        clientId: 'baff4f70-1ff2-44e6-a866-b272f8685bf1', // This is the ONLY mandatory field that you need to supply.
        authority: 'https://login.microsoftonline.com/88849b99-21a2-4b55-9f75-6c0f60759a3b', // Defaults to "https://login.microsoftonline.com/common"
        redirectUri: '/auth', // Points to window.location.origin by default. You must register this URI on Azure portal/App Registration.
        postLogoutRedirectUri: '/', // Points to window.location.origin by default.
        clientCapabilities: ['CP1'], // This lets the resource server know that this client can handle claim challenges.
    },
    cache: {
        cacheLocation: BrowserCacheLocation.LocalStorage, // Configures cache location. "sessionStorage" is more secure, but "localStorage" gives you SSO between tabs.
        storeAuthStateInCookie: isIE, // Set this to "true" if you are having issues on IE11 or Edge. Remove this line to use Angular Universal
    },
    system: {
        loggerOptions: {
            loggerCallback(logLevel: LogLevel, message: string) {
                console.log(message);
            },
            logLevel: LogLevel.Verbose,
            piiLoggingEnabled: false
        }
    }
}

export const protectedResources = {
    apiPolls: {
        endpoint: "https://localhost:7294/api/Poll",
        scopes: {
            read: ["api://baff4f70-1ff2-44e6-a866-b272f8685bf1/access_as_user"],
            write: ["api://baff4f70-1ff2-44e6-a866-b272f8685bf1/access_as_user"]
        }
    }
}

export const loginRequest = {
    scopes: []
};
