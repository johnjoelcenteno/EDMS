//Test if it's working
function cropSignature(imageDataUrl) {
    return new Promise((resolve, reject) => {
        const img = new Image();
        img.src = imageDataUrl;
        img.onload = () => {
            const canvas = document.createElement('canvas');
            const ctx = canvas.getContext('2d');
            const w = img.width;
            const h = img.height;
            const pixelData = ctx.getImageData(0, 0, w, h).data;

            let left = w, right = 0, top = h, bottom = 0;

            for (let y = 0; y < h; y++) {
                for (let x = 0; x < w; x++) {
                    const i = (y * w + x) * 4;
                    if (pixelData[i + 3] > 0) {
                        if (x < left) left = x;
                        if (x > right) right = x;
                        if (y < top) top = y;
                        if (y > bottom) bottom = y;
                    }
                }
            }

            const cropWidth = right - left + 1;
            const cropHeight = bottom - top + 1;

            canvas.width = cropWidth;
            canvas.height = cropHeight;
            ctx.drawImage(img, left, top, cropWidth, cropHeight, 0, 0, cropWidth, cropHeight);

            resolve(canvas.toDataURL('image/png'));
        };
        img.onerror = (error) => reject(error);
    });
}


function updateSignatureImage() {
    const signatureImage = document.querySelector('img');
    const imageDataUrl = signatureImage.src;
    cropSignature(imageDataUrl).then(croppedImageDataUrl => {
        signatureImage.src = croppedImageDataUrl;
    }).catch(console.error);
}