export default function discordLoader({ src, width }: { src: string; width: string | number }) {
  return `https://cdn.discordapp.com${src}?size=${width}`;
}
