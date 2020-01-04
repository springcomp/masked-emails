namespace masked_emails.Utils
{
    public static class ActionsHelper{
        public static bool TryParseEnum(string text, out Actions action)
        {
            action = Actions.Unrecognized;

            if (text == "m" || text == "me" || text == "my" || text == "my-profile" || text == "get-profile")
            {
                action = Actions.GetProfile;
                return true;
            }
            if (text == "c" || text == "create" || text == "create-address" || text == "create-masked-email-address")
            {
                action = Actions.CreateMaskedEmail;
                return true;
            }
            if (text == "d" || text == "delete" || text == "delete-address" || text == "delete-masked-email-address")
            {
                action = Actions.DeleteMaskedEmail;
                return true;
            }
            if (text == "l" || text == "list" || text == "get-addresses" || text == "get-masked-email-addresses")
            {
                action = Actions.GetMaskedEmailAddresses;
                return true;
            }
            if (text == "g" || text == "get" || text == "get-address" || text == "get-masked-email-address")
            {
                action = Actions.GetMaskedEmailAddress;
                return true;
            }

            if (text == "t" || text == "toggle" || text == "toggle-address" || text == "toggle-forwarding" || text == "toggle-masked-email-address-forwarding")
            {
                action = Actions.ToggleMaskedEmailAddressForwarding;
                return true;
            }

            if (text == "u" || text == "update" || text == "update-address" )
            {
                action = Actions.UpdateMaskedEmail;
                return true;
            }

            return false;
        }
    }
}