FROM node:20-alpine AS build

WORKDIR /app

COPY package.json package-lock.json* ./

RUN npm install

ENV REACT_APP_API_BASE_URL="http://localhost:5000"

COPY . .

RUN npm run build

EXPOSE 3000

CMD ["npm", "start"]