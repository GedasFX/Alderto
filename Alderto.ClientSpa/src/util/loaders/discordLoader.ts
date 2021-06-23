export default function discordLoader({ src, width }: { src: string; width: string | number }) {
  return `https://discordapp.com${src}?size=${width}`;
}
