# Get the NodeJS SDK to facilitate the building of the javaScript-Code
FROM node:20-alpine AS builder

#PROXY
ARG http_proxy
ARG https_proxy
ENV http_proxy=$http_proxy
ENV https_proxy=$https_proxy

# Copy all package and install all the dependencies
WORKDIR /app
COPY package*.json ./
RUN npm install

# Copy the rest of the UI source code
COPY . .
RUN npm run build

# Create a container to run the UI
FROM node:20-alpine
WORKDIR /app

# Copy necessary build files and dependencies from the builder stage
COPY --from=builder /app/.next ./.next
COPY --from=builder /app/node_modules ./node_modules
COPY --from=builder /app/package.json ./package.json
COPY --from=builder /app/public ./public

# Tell Docker that the app runs on port 3000
EXPOSE 3000
# Command to run the UI when the container starts
CMD ["npm", "start"]