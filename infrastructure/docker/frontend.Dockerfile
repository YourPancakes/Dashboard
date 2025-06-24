FROM node:18-alpine AS build
WORKDIR /app
COPY ["Frontend/package*.json", "./"]
RUN npm ci
COPY ["Frontend/", "./"]
RUN npm run build
FROM node:18-alpine AS runtime
WORKDIR /app
COPY --from=build /app/dist ./dist
RUN npm install -g serve
EXPOSE 80
CMD ["serve", "-s", "dist", "-l", "80"]
