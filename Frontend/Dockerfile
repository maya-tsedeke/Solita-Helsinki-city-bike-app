# Use official Node.js runtime as parent image
FROM node:18.12.1 AS build

# Set the working directory
WORKDIR /app
RUN npm install -g @angular/cli
# Copy the package.json and package-lock.json files
COPY ./package*.json ./

# Install dependencies
RUN npm install --force

# Copy the project files to the container
COPY / .

# Build the Angular app
 RUN ng build

# Use the official NGINX image as parent image
FROM nginx as runtime

# Copy the Angular build artifacts to NGINX html directory
COPY --from=build /app/dist/Frontend /usr/share/nginx/html

# Copy the NGINX configuration file to the container
COPY ./nginx.conf /etc/nginx/conf.d/default.conf


# Expose port 80 for the NGINX server
EXPOSE 80

# Start NGINX server
CMD ["nginx", "-g", "daemon off;"]
