<template>
  <v-container>
    <v-row class="text-center">
      <v-col class="mb-4">
        <h1 class="display-2 font-weight-bold mb-3">Welcome</h1>

        <p class="subheading font-weight-regular">Track your time easily.</p>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="12" md="6">
        <Form />
      </v-col>
      <v-col cols="12" md="6">
        <RightPanel />
      </v-col>
    </v-row>
    <v-divider class="mt-12 mb-12"></v-divider>
    <v-row class="text-center">
      <UploadImage />
    </v-row>
  </v-container>
</template>

<script>
import Form from "../components/Form"
import RightPanel from "../components/RightPanel"
import UploadImage from "../components/UploadImage"

export default {
  name: "Home",

  components: {
    Form,
    RightPanel,
    UploadImage
  },

  data: () =>
    UploadImage({
      //
    })
}
</script>
