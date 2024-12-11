# Secure Notes Frontend

SecureNote is an application for secure note keeping. This folder contains the frontend of the application.
This is a [Next.js](https://nextjs.org) project bootstrapped with [`create-next-app`](https://github.com/vercel/next.js/tree/canary/packages/create-next-app).

## Features

- The notes of the user are securely stored and accessed.
- Users can manage their notes via the usage of markdown language.
- The notes are automatically assigned tags upon saving.
- The user can create NSFW (Not Safe For Work) notes, which content is not immediately retrieved and displayed.

## Run Locally

Install dependencies

```bash
  npm install
```

Start the server

```bash
  npm run start
```


## Deployment

### NPM

```bash
  npm run deploy
```

### Docker
Build
```bash
docker build -t secure-note-frontend .
```
Run
```bash
docker run -p 3000:3000 secure-note-frontend
```
## Authors

- [@SergioMM0](https://github.com/SergioMM0)
- [@zosodk](https://github.com/zosodk)
- [@TawfikAzza](https://github.com/TawfikAzza)
- [@Ladam0203](https://github.com/Ladam0203)

