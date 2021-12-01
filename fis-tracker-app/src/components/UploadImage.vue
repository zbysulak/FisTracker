<template>
  <v-container class="example-simple">
    <h3 id="example-title" class="example-title m-4">Image upload</h3>
    <div class="upload">
      <ul>
        <li v-for="file in files" :key="file.id">
          <span>{{ file.name }}</span> - <span>{{ file.size }}</span> -
          <span v-if="file.error">{{ file.error }}</span>
          <span v-else-if="file.success">success</span>
          <span v-else-if="file.active">active</span>
          <span v-else></span>
        </li>
      </ul>
      <div class="example-btn">
        <v-btn class="mr-4">
          <FileUpload
            :post-action="fileUploadUrl"
            extensions="gif,jpg,jpeg,png,webp"
            accept="image/png,image/gif,image/jpeg,image/webp"
            :multiple="true"
            :size="1024 * 1024 * 10"
            v-model="files"
            ref="upload"
          >
            Select files
          </FileUpload>
        </v-btn>
        <v-btn
          v-if="!$refs.upload || !$refs.upload.active"
          @click.prevent="$refs.upload.active = true"
        >
          Start Upload
        </v-btn>
        <v-btn
          color="error"
          v-else
          @click.prevent="$refs.upload.active = false"
        >
          Stop Upload
        </v-btn>
      </div>
    </div>
    <br />
    <br />
    <br />
    <v-text-field contenteditable="true" @paste="onPaste">
      Paste Image
    </v-text-field>
  </v-container>
</template>
<style>
.example-simple label.btn {
  margin-bottom: 0;
  margin-right: 1rem;
}
</style>

<script>
import FileUpload from "vue-upload-component"
export default {
  name: "UploadImage",

  components: {
    FileUpload
  },

  data() {
    return {
      fileUploadUrl: this.appConfig.apiUrl + "/TimeInputs/parseimage",
      files: []
    }
  },

  methods: {
    onPaste(e) {
      console.log(e)
      let dataTransfer = e.clipboardData
      if (!dataTransfer) {
        return
      }
      this.$refs.upload.addDataTransfer(dataTransfer)
      console.log(dataTransfer)
    }
  }
}
</script>
