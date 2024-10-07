var currentPage = window.location.pathname;
var curLat, curLong;
$(document).ready(function () {
    // Check if the image was uploaded for this page and enable the "Next" button accordingly
    if (localStorage.getItem(currentPage + '_imageUploaded') === 'true') {
        $('#btn-next').prop('disabled', false);
    } else {
        $('#btn-next').prop('disabled', true);
    }

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            function (position) {
                curLat = position.coords.latitude;
                curLong = position.coords.longitude;
            }
        )
    }
});

$('#btn-back').on('click',function () {
     //Enable the "Next" button if an image was uploaded for this page
    if (localStorage.getItem(currentPage + '_imageUploaded') === 'true') {
        $('#btn-next').prop('disabled', false);
    }
});

let capturedImageElement; //variable to store the captured image element
document.getElementById('capture-button').addEventListener('change', function (event) {
    const file = event.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (event) {
            const imageDataURL = event.target.result;
            displayCapturedImage(imageDataURL);
        };
        reader.readAsDataURL(file);
    }
});
function displayCapturedImage(imageDataURL) {
    const image = new Image();
    image.src = imageDataURL;
    image.style.maxWidth = '100%';
    image.style.maxHeight = '100%';
    image.className = 'camera-preview';
    image.style.objectFit = 'cover';

    // Remove the previously captured image if exists
    if (capturedImageElement) {
        capturedImageElement.parentNode.removeChild(capturedImageElement);
    }

    // Append the image element to the leftBox div
    const leftBox = document.querySelector('.leftBox');
    leftBox.innerHTML = '';
    leftBox.appendChild(image);
    capturedImageElement = image;
}

document.getElementById('clear-button').addEventListener('click', clearImage);
// Function to clear the captured image
function clearImage() {
    // Remove the captured image element from the leftBox div
    if (capturedImageElement) {
        capturedImageElement.parentNode.removeChild(capturedImageElement);
        capturedImageElement = null;
    }
    $('#btn-next').prop('disabled', true);
    localStorage.setItem(currentPage + '_imageUploaded', 'false');
}

// To display information to user while uploading image
var uploadingToast;
function showUploading() {
    toastr.options.positionClass = 'toast-bottom-right';
    toastr.options.timeOut = 0;
    uploadingToast = toastr.info('Wait..! image is uploading...');
}
function hideUploading() {
    toastr.clear(uploadingToast);
    toastr.options.timeOut = 5000;
}

document.getElementById('upload-capture').addEventListener('click', function () {
    if (capturedImageElement) {

        showUploading();

        var rootPath = window.location.origin;

        // Create a canvas element to draw the image
        var canvas = document.createElement('canvas');
        var ctx = canvas.getContext('2d');

        // Set the maximum dimensions for the resized image
        var maxWidth = 800;
        var maxHeight = 600;

        // Calculate new dimensions while preserving aspect ratio
        var width = capturedImageElement.naturalWidth;
        var height = capturedImageElement.naturalHeight;
        if (width > height) {
            if (width > maxWidth) {
                height *= maxWidth / width;
                width = maxWidth;
            }
        } else {
            if (height > maxHeight) {
                width *= maxHeight / height;
                height = maxHeight;
            }
        }
        canvas.width = width;
        canvas.height = height;

        // Draw the resized image onto the canvas
        ctx.drawImage(capturedImageElement, 0, 0, width, height);

        // To give upload date time and location watermark to image
        const currentDate = new Date();
        const day = String(currentDate.getDate()).padStart(2, '0');
        const month = String(currentDate.getMonth() + 1).padStart(2, '0'); // Months are zero-based
        const year = currentDate.getFullYear();
        let hours = currentDate.getHours();
        const minutes = String(currentDate.getMinutes()).padStart(2, '0');
        const seconds = String(currentDate.getSeconds()).padStart(2, '0');
        const ampm = hours >= 12 ? 'PM' : 'AM';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'

        const strTime = `${hours}:${minutes}:${seconds} ${ampm}`;
        const dateString = `${day}/${month}/${year}, ${strTime}`;
        var lati;
        var longi;
        if (curLat !== null && curLat !== "" && curLat !== undefined && curLong !== null && curLong !== "" && curLong !== undefined) {
            lati = `Latitude: ${curLat}`;
            longi = `Longitude: ${curLong}`;
        } else {
            lati = "Latitude: 0.0";
            longi = "Longitude: 0.0";
        }

        ctx.font = '13px Arial';
        ctx.fillStyle = 'white';
        ctx.strokeStyle = 'black';
        ctx.lineWidth = 2;

        const textX = 10;
        const lineHeight = 20;

        ctx.strokeText(dateString, textX, height - 45);
        ctx.fillText(dateString, textX, height - 45);
        ctx.strokeText(lati, textX, height - 25);
        ctx.fillText(lati, textX, height - 25);
        ctx.strokeText(longi, textX, height - 5);
        ctx.fillText(longi, textX, height - 5);

        // Convert the canvas content to a blob
        canvas.toBlob(function (blob) {
            // Create a FormData object to send the image data
            var formData = new FormData();
            formData.append('file', blob, 'captured_image.jpg');

            var imageSpec = $('#image-position').text().trim();
            formData.append('imageDetails', imageSpec);

            // Send a POST request
            fetch(rootPath + '/VIR/Upload/UploadVehicleDocuments', {
                method: 'POST',
                body: formData
            })
                .then(response => response.json())
                .then(data => {
                    hideUploading();
                    if (data.success == true) {
                        toastr.options.positionClass = 'toast-bottom-right';
                        toastr.success('Image uploaded successfully');

                        $('#btn-next').prop('disabled', false);
                        localStorage.setItem(currentPage + '_imageUploaded', 'true');
                    }
                    else {
                        toastr.options.positionClass = 'toast-bottom-right';
                        toastr.error(data.message);
                    }
                })
                .catch(error => {
                    hideUploading();
                    toastr.options.positionClass = 'toast-bottom-right';
                    toastr.error('An error occurred while uploading the image' + error);
                });
        }, 'image/jpeg',1.0);
    }
});
