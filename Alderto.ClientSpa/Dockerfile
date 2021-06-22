FROM node:lts-alpine AS node_modules
WORKDIR /build

# Install ONLY the packages needed for SSR.
COPY package.json .
COPY yarn.lock .
RUN [ "yarn", "install", "--prod" ]


FROM node:lts-alpine AS build
WORKDIR /build

# Install all of the packages needed for the build.
COPY package.json .
COPY yarn.lock .

# Copy the node_modules needed for production to speed up the installation.
COPY --from=node_modules /build/node_modules ./node_modules
RUN [ "yarn", "install" ]

# Copy the source files and build the program.
COPY . .
RUN [ "yarn", "build" ]


FROM node:lts-alpine AS prod
WORKDIR /app

# Enable production optimizations
ENV NODE_ENV=production

# Copy the files required for rendering.
COPY package.json .
COPY --from=build /build/.next ./.next
COPY --from=build /build/public ./public
COPY --from=node_modules /build/node_modules ./node_modules

EXPOSE 3000
ENTRYPOINT [ "yarn", "start" ]