// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

class PushNotificationApp {
    domDiv;
    window;
    serviceWorker;
    subscribeContainer;
    notifyContainer;
    subscriptionTemplate;
    sendNotificationTemplate;

    subscription;
    subscriptionInfo;
    notificationForm;
    subscribeButton;

    applicationServerKey = this.urlBase64ToUint8Array('BAgyTA0ivcjKKUrUxtZ-TrpZHsX-934jjEQXC2qnQCAWKN-wguiObwK1lGGU2y3oduOob5bRPjM6CFgtmKkMh2A');

    constructor(window, serviceWorker, subscribeContainer, notifyContainer, subscriptionTemplate, sendNotificationTemplate) {
        this.window = window;
        this.serviceWorker = serviceWorker;
        this.subscribeContainer = subscribeContainer;
        this.notifyContainer = notifyContainer;
        this.subscriptionTemplate = subscriptionTemplate;
        this.sendNotificationTemplate = sendNotificationTemplate;

        if (!this.supportsPushNotifications()) {
            this.subscribeContainer.textContent = "No support for push notifications.";
            return;
        }

        this.subscribeContainer.appendChild(this.subscriptionTemplate.content);
        this.notifyContainer.appendChild(this.sendNotificationTemplate.content);

        this.subscribeButton = this.subscribeContainer.querySelector('button');
        this.subscribeButton.addEventListener('click', this.handleClick.bind(this));

        this.subscriptionInfo = this.subscribeContainer.querySelector('.subscription-info');
        this.notificationForm = this.notifyContainer.querySelector('.notification-form');

        this.serviceWorker.pushManager.getSubscription().then(subscription => {
            console.log(subscription);
        
            if (!subscription) {
                this.hideSubscriptionInfo();
                this.hideNotificationForm();
                this.showSubscribeButton();
                return Promise.resolve();
            }
        
            console.log(subscription);

            console.log(new Uint8Array(subscription.options.applicationServerKey));
            console.log(this.applicationServerKey);

            console.log(window.base64u8ArrayBuffer.arrayBufferToBase64(subscription.options.applicationServerKey) == window.base64u8ArrayBuffer.uint8ArrayToBase64(this.applicationServerKey));
        
            if (base64u8ArrayBuffer.arrayBufferToBase64(subscription.options.applicationServerKey) == base64u8ArrayBuffer.uint8ArrayToBase64(this.applicationServerKey)) {
                this.subscription = subscription;
        
                this.populateSubscriptionInfo();
        
                this.showSubscriptionInfo();
                this.showNotificationForm();
                this.hideSubscribeButton();
                return Promise.resolve();
            }
        
            this.hideSubscriptionInfo();
            this.hideNotificationForm();
            this.showSubscribeButton();
            //return subscription.unsubscribe();
        
        });
    }

    hideSubscriptionInfo() {
        this.subscriptionInfo.classList.add('d-none');
    }
    hideNotificationForm() {
        this.notificationForm.classList.add('d-none');
    }
    hideSubscribeButton() {
        this.subscribeButton.classList.add('d-none');
    }

    showSubscriptionInfo() {
        this.subscriptionInfo.classList.remove('d-none');
    }
    showNotificationForm() {
        this.notificationForm.classList.remove('d-none');
    }
    showSubscribeButton() {
        this.subscribeButton.classList.remove('d-none');
    }

    populateSubscriptionInfo() {
        if (!this.subscription) {
            return;
        }

        this.subscriptionInfo.innerHTML = `<p>Your subscription: ${JSON.stringify(this.subscription)}</p>`;
    }

    supportsPushNotifications() {
        if (!('serviceWorker' in this.window.navigator)) {
            return false;
        }

        if (!('PushManager' in this.window)) {
            return false;
        }

        return true;
    }

    urlBase64ToUint8Array(base64String) {
        var padding = '='.repeat((4 - base64String.length % 4) % 4);
        var base64 = (base64String + padding)
            .replace(/\-/g, '+')
            .replace(/_/g, '/');

        var rawData = window.atob(base64);
        var outputArray = new Uint8Array(rawData.length);

        for (var i = 0; i < rawData.length; ++i) {
            outputArray[i] = rawData.charCodeAt(i);
        }
        return outputArray;
    }

    askPermission() {
        return new Promise((resolve, reject) => {
            const permissionResult = this.window.Notification.requestPermission((result) => resolve(result));

            if (permissionResult) {
                permissionResult.then(resolve, reject);
            }
        }).then(permissionResult => {
            if (permissionResult !== 'granted') {
                throw new Error("We weren't granted permission.'");
            }
        });
    }

    subscribe() {
        const subscribeOptions = {
            userVisibleOnly: true,
            applicationServerKey: this.applicationServerKey
        };

        return this.serviceWorker.pushManager.getSubscription().then(subscription => {
            if (!subscription) {
                return Promise.resolve();
            }

            if (subscription.applicationServerKey == this.applicationServerKey) {
                return Promise.resolve();
            }

            return subscription.unsubscribe()

        }).then(() => this.serviceWorker.pushManager.subscribe(subscribeOptions));
    }

    registerSubscription(subscription) {
        console.log('Received PushSubscription: ',
            JSON.stringify(subscription));

        return fetch('/Home/RegisterSubscription', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': window.webPushNotifications.antiforgeryToken
            },
            body: JSON.stringify(subscription)
        }).then(response => {
            if (!response.ok) {
                throw new Error('Bad status code from server.');
            }

            return response.json();
        }).then(data => {
                if (!data.success) {
                    throw new Error('Bad response from server.');
                }
                return data
            })
    }

    handleClick(event) {
        this.askPermission()
            .then(this.subscribe.bind(this))
            .then(this.registerSubscription.bind(this))
            .then(data => {
                console.log(data);

                this.subscription = data;

                this.populateSubscriptionInfo();

                this.showSubscriptionInfo();
                this.showNotificationForm();
                this.hideSubscribeButton();
            })
            .catch(err => console.log(err));
    }

    generateButton() {
        let b = document.createElement('button');
        b.textContent = "Subscribe to Push Notificatons";

        b.addEventListener('click', this.handleClick.bind(this));

        return b;
    }

    insertButton() {
        let b = this.generateButton();

        this.domDiv.appendChild(b);
    }
}

function registerServiceWorker() {
    return window.navigator.serviceWorker
        .register('/service-worker.js')
        .then((registration) => {
            console.log('Service worker successfully registered.');
            return registration;
        })
        .catch((err) => {
            console.error('Unable to register service worker.', err);
        }); 
}

(function (document, window) {
    registerServiceWorker();

    window.navigator.serviceWorker.ready.then(registration => {
        const subscribeContainer = document.getElementById('subscribe');
        const notifyContainer = document.getElementById('notify');
        const subscriptionTemplate = document.getElementById('subscription');
        const sendNotificationTemplate = document.getElementById('send-notification');

        new PushNotificationApp(window, registration, subscribeContainer, notifyContainer, subscriptionTemplate, sendNotificationTemplate);
        //document.querySelectorAll('.subscribe-button').forEach(e => {
        //    new PushNotificationSubscribeButton(e, window, registration);
        //});
    });

    document.getElementById('log-subscription').addEventListener('click', e => {
        window.navigator.serviceWorker.ready.then(registration => {
            console.log(registration);
            registration.pushManager.getSubscription().then(subscription => {
                console.log(subscription);
            });

            registration.pushManager.permissionState().then(state => console.log(state));
        });
    });
})(document, window);