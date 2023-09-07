// Register event listener for the 'push' event.
self.addEventListener('push', function (event) {
    const data = event.data?.json() ?? {};
    const title = data.title || 'ServiceWorker Cookbook';
    const message =
        data.body || 'Alea iacta est';

    // Keep the service worker alive until the notification is created.
    event.waitUntil(
        // Show a notification with title 'ServiceWorker Cookbook' and body 'Alea iacta est'.
        self.registration.showNotification(title, {
            body: message
        })
    );
});