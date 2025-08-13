# Use official Node.js image for build
FROM node:latest AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY ./src ./src
COPY ./public ./public

RUN npm run build

# Use official Nginx image for serving static files
FROM nginx:stable-alpine
COPY --from=build /app/build /usr/share/nginx/html

# copy to inject config
COPY env.template.js /usr/share/nginx/html/env.template.js
COPY entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80

HEALTHCHECK CMD wget -qO- http://localhost:80/ || exit 1

ENTRYPOINT ["/entrypoint.sh"]