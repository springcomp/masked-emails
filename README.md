# MaskedEmail

This repository hosts the main application and API for the "masked emails".

![image](https://user-images.githubusercontent.com/8488398/71770833-2bec5400-2f32-11ea-84c5-274bfd161754.png)

## Quick Overview

To prevent SPAM, I wanted to be able to create as many single-purpose email addresses as requested.

A lot of solutions exist, to create on-the-fly disposable email addresses, such as [Mailinator](https://www.mailinator.com/) or [10MinuteMail](https://10minutemail.com/10MinuteMail/index.html) but I do not want my email addresses to be disposable.

Also, free disposable email address services usually do not come with any concerns with regards to privacy or security.

I also evaluated other solutions such as [MySudo](https://mysudo.com/), but each had drawbacks (like, only running on iOS for instance)

The solution that most closely resemble what I want is a service like [Abine Blur](https://dnt.abine.com/#register) that provides exactly the kind of features I need:

For instance, in Abine Blur, I have the following feature:

- Do not have security risks shared by other anonymous free disposable email services.
- Create as many single-purpose email addresses as required.
- Each masked email address redirects to a hidden, "real" email address, never revealed to spammers.
- Each masked address can be disabled, thus staying active but not relaying messages to the target "real" email address.
- A masked email address can be deleted altogether when no longer used.

Another service that looks similar (but that I have not evaluated) is offered by [FairCustodian](https://nope.faircustodian.com/).

Those services look promising, and I actually used (and still use) Abine Blur. However, the _key_ missing feature to me is the ability to *send* or *reply to* an email using one of the masked email addresses.

## Let’s build an open-source alternative

Because I **really** wanted this feature, I started to build it myself.

The "masked emails" solution revolves around three components:

1. A backend self-hosted email server. This is a real full-featured SMTP / IMAP mail server hosted on a private virtual server using my own domain.  
For this, I use the [fullstack but simple mailserver](https://github.com/tomav/docker-mailserver) implementation provided by [tomav](https://github.com/tomav) as a Docker image.

Because this is a real email server, I can fire up any mail client that I want to actually send an email from any one of the custom masked email address at any time.

For this, I use the excellent [ThunderBird Portable](https://portableapps.com/apps/internet/thunderbird_portable) on a USB stick that I keep with me all the time.

2. A set of PowerShell scripts and CRON jobs are used to administer the masked email addresses (CRUD) as well as periodically redirect mail messages to a "read" configurable email address.  
Messages from a disabled masked email address are periodically deleted (after a configurable delay that defaults to two days).  
Messages from an active masked email address are redirected to a target email address.

The source code for these scripts is hosted on the [masked-emails-pwsh](https://github.com/springcomp/masked-emails-pwsh) repository.

The scripts are available as a Debian package thanks to a continuous integration using AppVeyor.

3. A backend service, that runs on the Linux Virtual Private Server. This service listens on an Azure Storage Queue for commands coming from a front-end application.  
Those commands are used to disable or delete an existing masked email address’ mailbox from the mail server. Those commands are also used to create a new masked email address mailbox.

The source code for this daemon is hosted on the [masked-emails-daemon](https://github.com/springcomp/masked-emails-daemon) repository.

The daemon is available as a Debian package thanks to a continuous integration using AppVeyor.

4. A front-end web application and API. This application is the purpose of *this* repository.

These are actually two distinct applications,
- one hosting the API and the Angular-based front-end.
- the other, hosting a custom IdentityServer4-based Identity Provider.

The front-end communicates with the backend daemon service using an Azure Storage queue.
