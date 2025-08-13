# Use official Node.js image for build
FROM node:latest AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# Use official Nginx image for serving static files
FROM nginx:stable-alpine
WORKDIR /usr/share/nginx/html
COPY --from=build /app/build .
# Copy custom nginx config if needed
# COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
