ARG TARGETPLATFORM
ARG BUILDPLATFORM

# Use official Node.js image for build (honor the build platform)
FROM --platform=$BUILDPLATFORM node:20-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm install --legacy-peer-deps
COPY ./src ./src
COPY ./public ./public

RUN npm run build

# Use official Nginx image for serving static files, targeting the requested platform (e.g., linux/arm64)
FROM --platform=$TARGETPLATFORM nginx:stable-alpine
COPY --from=build /app/build /usr/share/nginx/html

# copy to inject config
COPY env.template.js /usr/share/nginx/html/env.template.js
COPY --chmod=0755 entrypoint.sh /entrypoint.sh

COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80

ENTRYPOINT ["/entrypoint.sh"]