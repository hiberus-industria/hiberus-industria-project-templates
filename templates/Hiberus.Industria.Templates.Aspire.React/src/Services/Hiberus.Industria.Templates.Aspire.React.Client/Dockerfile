# --- Build Stage ---
# Uses a lightweight Node.js image with Yarn pre-installed.
FROM node:22.15.0-alpine AS builder

# Set the working directory inside the container.
WORKDIR /app

# Only copy the specific SPA application folder.
COPY src/Services/volkswagen.picking.client/ ./

# Install application dependencies.
RUN yarn install

# Execute the build script defined in package.json.
# This will generate the optimized static files in the 'dist' directory.
RUN yarn build

# --- Serve Stage ---
# Uses a very lightweight Nginx image to serve the static files.
FROM nginx:alpine AS production

# Copy the custom Nginx configuration.
# This configuration is crucial for SPAs, as it redirects all routes to index.html.
COPY --from=builder /app/nginx.conf /etc/nginx/conf.d/default.conf

# Copy the compiled static files from the 'builder' stage to the Nginx serving directory.
# The 'dist' directory is where 'vite build' saves the compiled output.
COPY --from=builder /app/dist /usr/share/nginx/html

# Create a startup script to generate the runtime config and start Nginx.
RUN echo 'envsubst < /usr/share/nginx/html/runtime-config.template.js > /usr/share/nginx/html/runtime-config.js && nginx -g "daemon off;"' > start.sh
RUN chmod +x start.sh

# Expose port 80, which is the default Nginx port.
EXPOSE 80

# Start the application.
CMD ["/bin/sh", "-c", "/start.sh"]
