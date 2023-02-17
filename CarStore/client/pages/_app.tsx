import "../styles/globals.css";
import type { AppProps } from "next/app";
import { useEffect } from "react";
import { SessionProvider } from "next-auth/react";

import { ReactQueryDevtools } from "react-query/devtools";
import { Hydrate, QueryClient, QueryClientProvider } from "react-query";
import { useState } from "react";
import { ThemeProvider } from "@mui/material";
import theme from "../styles/theme";
import createEmotionCache from "../lib/createEmotionCache";
import { CacheProvider } from "@emotion/react";

const clientSideEmotionCache = createEmotionCache();

function MyApp({
  Component,
  emotionCache = clientSideEmotionCache,
  pageProps: { session, ...pageProps } }: AppProps
) {
  useEffect(() => {
    const jssStyles = document.querySelector("#jss-server-side");
    if (jssStyles) {
      jssStyles.parentElement?.removeChild(jssStyles);
    }
  }, []);

  const [queryClient] = useState(() => new QueryClient());

  return (
    // <SessionProvider session={session}>
    //   <Component {...pageProps} />
    // </SessionProvider>

    <QueryClientProvider client={queryClient}>
      <Hydrate state={pageProps.dehydratedState}>
        <SessionProvider session={session}>
          <CacheProvider value={emotionCache}>
            <ThemeProvider theme={theme}>
              <Component {...pageProps} />
            </ThemeProvider>
          </CacheProvider>
        </SessionProvider>
        <ReactQueryDevtools initialIsOpen={false}></ReactQueryDevtools>
      </Hydrate>
    </QueryClientProvider>
  );
}

export default MyApp;
