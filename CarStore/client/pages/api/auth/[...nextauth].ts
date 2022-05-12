import { decode } from "jsonwebtoken";
import NextAuth from "next-auth";
import { DefaultJWT } from "next-auth/jwt";
import IdentityServer4Provider from "next-auth/providers/identity-server4";

export interface SessionJWTPayload {
  given_name: string;
  family_name: string;
  extension_UserType: string;
  newUser: boolean;
  emails: Array<string>;
}

export default NextAuth({
  // Configure one or more authentication providers
  providers: [
    IdentityServer4Provider({
      id: "identity-server4",
      authorization: {
        params: {
          scope: "openid profile email",
          response_type: "code",
          prompt: "login",
        },
        url: process.env.ID4_ISSUER,
      },
      issuer: process.env.ID4_ISSUER,
      // @ts-ignore
      clientId: process.env.ID4_CLIENT_ID,
      clientSecret: process.env.ID4_SECRET,
    }),
  ],
  callbacks: {
    async session({ session, token, user }) {
      if (!token) {
        return session;
      }
      const decodedIdToken = decode(token.access_token as string) as DefaultJWT;
      session.user = decodedIdToken;
      return session;
    },
    async jwt({ token, account, user }) {
      if (account) {
        token = account;
      }
      return token;
    },
  },
});
