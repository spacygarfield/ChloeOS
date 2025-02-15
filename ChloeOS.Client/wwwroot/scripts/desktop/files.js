$(`#desktop`).click(unselectFile);

const $files = $(`.file`);
$files
    .click(selectFile)
    .on(`dblclick`, openFile);

let $selectedFile;

function selectFile(e) {
    if ($selectedFile) {
        return;
    }

    e.preventDefault();
    e.stopPropagation();

    $selectedFile = $(e.currentTarget);
    $selectedFile.addClass(`selected`);
}

function unselectFile(e) {
    const $selected = $(e.currentTarget);

    if (!$selectedFile || $selected.is($selectedFile) || $selected.hasClass(`file`)) {
        return;
    }

    $selectedFile.removeClass(`selected`);
    $selectedFile = undefined;
}

function openFile() {
    if (!$selectedFile) {
        return;
    }

    const uri = `/fs/${$selectedFile.attr(`data-file-id`)}`;
    openWindow(uri);
}