$(`[data-settings]`).click(openSettings);

function openSettings() {
    const uri = `/settings/${$(this).attr(`data-settings`)}`;
    if (isSettingsOpen(uri)) {
        return;
    }

    openWindow(uri);
}

function isSettingsOpen(uri) {
    const $window = $(`.window iframe[src="${uri}"]`);
    return !$window;
}