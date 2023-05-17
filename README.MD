

### Docker image for testing linux

To build the image
>docker build -t magicapp .

Replace magicapp with the image id and add optional tag
ex: docker build -t netloader:101 .

List all images
>docker images

Run the image

>docker run -p 8080:80 {imageid}

