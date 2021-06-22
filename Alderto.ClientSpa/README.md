# Next.js starter by GedasFX

This starter extends `npx create-next-app` and consists only of the packages, I would want to see across all of my projects. For more advanced and narrowed down use cases check out other branches.

Further below I will provide what was changed from regular `npx create-next-app`.

## TypeScript

By far most important change is the fact that TypeScript is enabled from the start. To accommodate this change, [`tsconfig.json`](tsconfig.json) was added, alongside the generated [`next-env.d.ts`](next-env.d.ts). 

This required [`@types/node`](https://www.npmjs.com/package/@types/node),  [`@types/react`](https://www.npmjs.com/package/@types/react),  and [`typescript`](https://www.npmjs.com/package/typescript) packages to be added to `devDependencies`.

## Linting and code formatting

For enforcing code quality, 3 tools were used. [ESLint](https://marketplace.visualstudio.com/items?itemName=dbaeumer.vscode-eslint), [Prettier](https://marketplace.visualstudio.com/items?itemName=esbenp.prettier-vscode), and [Code Spell Checker](https://marketplace.visualstudio.com/items?itemName=streetsidesoftware.code-spell-checker), as seen in the [`.vscode`](.vscode) folder. 

### ESLint

A preconfigured [`.eslintrc`](.eslintrc) file was added in the root directory. Feel free to customize rules or any other part however you like.

This required [`@typescript-eslint/eslint-plugin`](https://www.npmjs.com/package/@typescript-eslint/eslint-plugin), [`@typescript-eslint/parser`](https://www.npmjs.com/package/@typescript-eslint/parser), [`eslint`](https://www.npmjs.com/package/eslint), [`eslint-plugin-jsx-a11y`](https://www.npmjs.com/package/eslint-plugin-jsx-a11y), [`eslint-plugin-react`](https://www.npmjs.com/package/eslint-plugin-react), and [`eslint-plugin-react-hooks`](https://www.npmjs.com/package/eslint-plugin-react-hooks) packages to be added to `devDependencies`.

### Prettier

The code formatter of choice for this starter is Prettier. Feel free to customize [`.prettierrc`](.prettierrc), and [`.prettierignore`](.pterrierignore) files to your likely.

This required [`prettier`](https://www.npmjs.com/package/prettier) package to be added to `devDependencies`.

## package.json

### Dependencies

One of most important goals of making the starter is reducing Docker image size as much as possible. This is why pretty much all of the packages are placed in `devDependencies` section. These packages are only pulled for building of static files (`next build`), and will not be bundled in the final image.

The only packages to be placed in `dependencies` are the ones needed for SSR itself. By default there are only 3: `next`, `react`, and `react-dom`. 

If you do not care about docker image size or will not use it all, you can ignore this rule.


### Scripts

Normal `npx create-next-app` generates `dev`, `build`, and `start` scripts. In addition to those, I added [`dev:debug`](https://nextjs.org/docs/advanced-features/debugging), [`build:analyze`](https://www.npmjs.com/package/@next/bundle-analyzer), [`export`](https://nextjs.org/docs/advanced-features/static-html-export), [`type-check`](https://www.typescriptlang.org/docs/handbook/compiler-options.html), [`lint`](https://eslint.org/docs/user-guide/command-line-interface), and [`format`](https://prettier.io/docs/en/cli.html). You can click on any of these links to inspect what they do. I found that I use those commands often, and found them useful to add to a fresh project.

## Docker Support

This starter ships with a [`Dockerfile`](Dockerfile) used for production builds. For development refer to [Remote - Containers support](#remote-containers-support)

### Production

A lot of effort was put in to optimize build times and the final image size. That is why we ended up with such a weird [`Dockerfile`](Dockerfile), however I can assure you every part of this [multi-stage Dockerfile](https://docs.docker.com/develop/develop-images/multistage-build/) makes sense.

**Step 1:** Installing (and caching) production `node_modules`.

```dockerfile
FROM node:lts-alpine AS node_modules
WORKDIR /build

# Install ONLY the packages needed for SSR.
COPY package.json .
COPY yarn.lock .
RUN [ "yarn", "install", "--prod" ]
```

**Step 2:** Building the application and rendering static pages.

```dockerfile
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
```

To speed up the installation `node_modules` we copy production-only packages to our new node_modules folder and then letting `yarn` to deal with fetching the rest of the packages from `devDependencies`.

**Step 3:** Preparing a space-conscious production image.

```dockerfile
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
```

Alternatively, if the site is entirely static, on step 2 `yarn export` could be used at the end and only the static files be copied to an `nginx` (or equivalent) image's public directory.

### Remote Containers Support

For development purposes, you could make use of VSCode's [Remote - Containers](https://code.visualstudio.com/docs/remote/containers) plugin to set up dependencies in a docker container. All you need to do is launch the folder in container as seen in the docs.

## PWA Support

This starter comes with basic PWA support. The default `<meta>` tags are described in [`_app.tsx`](src/pages/_app.tsx) file. The service worker itself is managed by [`next-pwa`](https://www.npmjs.com/package/next-pwa) plugin, and it is possible to change the runtime caching rules on the [`next.config.js`](next.config.js) file. 	

